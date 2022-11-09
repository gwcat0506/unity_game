using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    
    public float dropSpeed = -0.03f;

    void Update()
    {
        // 아이템을 매 프레임마다 조금씩 아래 방향으로 이동시킴
        transform.Translate(0,this.dropSpeed,0);
        if (transform.position.y < -1.0f )
        {
            Destroy(gameObject);
        }
    }
}
