using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray : MonoBehaviour
{
    public Vector2 movement;
    public Vector2 temp;
    private void OnEnable()
    {
        temp = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Exists &&
            Player.Instance.isSaMu && 
            (!Player.Instance.isFight ||
            Player.Instance.transform.Find("Role").Find("samu").GetComponent<samu>().L_energy <= 0) &&
            transform.localScale.y > 0f)
        {
            transform.localScale -= new Vector3(0, 1f * Time.deltaTime);
        }
        else if(!Player.Instance.isSaMu || transform.localScale.y <= 0)
        {
            gameObject.SetActive(false);
            transform.localScale = temp;
        }
    }
}
