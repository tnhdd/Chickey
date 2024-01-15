using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    private Image barImage;
    private Mana mana;

    private void Awake()
    {
        barImage = transform.Find("bar").GetComponent<Image>();
        mana = new Mana();
    }

    private void Update()
    {
        mana.Update();
        barImage.fillAmount = mana.GetManaNormalized();

        if (Input.GetMouseButtonDown(0))
        {
            // Call the function to spend mana
            mana.SpendMana(30f); // Adjust the amount as needed
        }
    }

}
public class Mana
{
    private const int mana_max = 100;
    private float manaAmount;
    private float manaRegenAmount;

    public Mana()
    {
        manaAmount = 0;
        manaRegenAmount = 30f;
    }

    public void Update()
    {
        manaAmount += manaRegenAmount * Time.deltaTime;
        manaAmount = Mathf.Clamp(manaAmount, 0f, mana_max);
    }
    public void SpendMana(float amount)
    {
        if (manaAmount >= amount)
        {
            manaAmount -= amount;
        }
    }

    public float GetManaNormalized()
    {
        return manaAmount / mana_max;
    }
}
