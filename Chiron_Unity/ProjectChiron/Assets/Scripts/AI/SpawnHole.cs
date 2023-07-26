using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHole : MonoBehaviour
{
    public float maxWidth = 1;
    public float openSpeed = 1;

    private float currentScale = 0;

    private float direction = 1;
    // Start is called before the first frame update
    void Start()
    {
        //currentScale = transform.localScale.x;
        currentScale = 0;
        SetScale(0);
        //OpenAndClose();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

//    public void OpenAndClose()
//    {
//        StartCoroutine(CoOpenAndClose());
//    }

//    private IEnumerator CoOpenAndClose()
//    {
//        yield return new WaitForSeconds(2);
//        yield return CoScale();
//        yield return new WaitForSeconds(1);
//        yield return CoScale(true);
//    }
//    
//    public void ScaleHole(bool close = false)
//    {
//        StartCoroutine(CoScale(close));
//    }
    
    public IEnumerator CoScale(bool close = false)
    {
        direction = close ? -1 : 1; 
        bool b = false;
        while (!b)
        {
            b = Scale();
            yield return null;
        }
    }
    
    private bool Scale()
    {
        float nextScale = currentScale + Time.deltaTime * openSpeed * direction;
        bool finished = direction > 0 ? nextScale >= maxWidth : nextScale <= 0;
        if (finished)
        {
            if (direction > 0)
            {
                currentScale = maxWidth;
                nextScale = maxWidth;
            }
            else
            {
                currentScale = 0;
                nextScale = 0;
            }
            SetScale(nextScale);
            return true;
        }

        currentScale = nextScale;
        SetScale(nextScale);
        return false;
        //transform.localScale.x;
    }

    private void SetScale(float f)
    {
        transform.localScale = new Vector3(f,transform.localScale.y,f);
    }
}
