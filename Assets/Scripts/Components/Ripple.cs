using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ripple : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(GameManager.Exists &&
            Player.Instance.isSaMu &&
            transform.localScale.x < 2f)
        {
            transform.localScale += new Vector3(1f * Time.deltaTime, 1f * Time.deltaTime);
        }
        else
        {
            gameObject.SetActive(false);
            Player.Instance.ReturnPooledObject(gameObject, Player.Instance.Ripple);
        }
    }
}
