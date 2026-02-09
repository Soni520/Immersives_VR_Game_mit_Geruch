using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CircleBreathUI : MonoBehaviour
{
    public Image circleImage;
    public TextMeshProUGUI breathText;

    public float breathDuration = 4f;

    float timer;
    bool breathingIn = true;

    void Start()
    {
        timer = breathDuration;
        breathText.text = "Breath In";
        circleImage.fillAmount = 1f;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        circleImage.fillAmount = timer / breathDuration;

        if (timer <= 0f)
        {
            breathingIn = !breathingIn;

            breathText.text = breathingIn ? "Breath In" : "Breath Out";

            timer = breathDuration;
            circleImage.fillAmount = 1f;
        }
    }
}
