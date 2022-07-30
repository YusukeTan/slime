using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueCanvasController : MonoBehaviour
{
//    public Animator animator;
    public Text dialogText;

    public static DialogueCanvasController instance = null;

//    [SerializeField] private GameObject dialog;

    protected Coroutine m_DeactivationCoroutine;
    protected readonly int m_HashActivePara = Animator.StringToHash("Active");


    void Awake()
    {
        
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        
    }


    IEnumerator SetAnimatorParameterWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
//        animator.SetBool(m_HashActivePara, false);
        //        gameObject.SetActive(false);
        //        dialog.SetActive(false);
        dialogText.text = "";

    }

    public void ActivateCanvasWithText(string text)
    {

        /*
        if (m_DeactivationCoroutine != null)
        {
            StopCoroutine(m_DeactivationCoroutine);
            m_DeactivationCoroutine = null;
        }
        */

//        gameObject.SetActive(true);
//        dialog.SetActive(true);
//        animator.SetBool(m_HashActivePara, true);
        dialogText.text = text;
    }


    public void DeactivateCanvasWithDelay(float delay)
    {
        m_DeactivationCoroutine = StartCoroutine(SetAnimatorParameterWithDelay(delay));
    }

}
