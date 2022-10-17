using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageExpressions : MonoBehaviour
{

    [SerializeField]
    private Sprite worried, smile;

    [SerializeField]
    private Transform pole, poleTip;

    [SerializeField]
    private Transform leftEye, rightEye;
    private Transform leftPupil, rightPupil;

    private SpriteRenderer mouth;
    // Start is called before the first frame update
    void Start()
    {
        mouth = transform.Find("Mouth").GetComponent<SpriteRenderer>();
        leftPupil = leftEye.GetChild(0);
        rightPupil = rightEye.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (pole.eulerAngles.z > 10 && pole.eulerAngles.z < 100 || pole.eulerAngles.z < 350 && pole.eulerAngles.z > 200)
            mouth.sprite = worried;
        else
            mouth.sprite = smile;

        leftPupil.localPosition = new Vector3(0, 0, -0.02f);
        rightPupil.localPosition = new Vector3(0, 0, -0.02f);

        Vector3 leftEyeDirection = (poleTip.position - leftEye.position).normalized;
        Vector3 rightEyeDirection = (poleTip.position - rightEye.position).normalized;

        leftEyeDirection.z = 0;
        rightEyeDirection.z = 0;

        leftPupil.localPosition += leftEyeDirection * 0.15f;
        rightPupil.localPosition += rightEyeDirection * 0.15f;
    }
}
