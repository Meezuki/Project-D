using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text Mana;
    [SerializeField] private SpriteRenderer imageSR;
    [SerializeField] private GameObject wrapper;
    public Card Card { get; private set; } //reference card for later
    public void Setup(Card card)
    {
        Card = card;
        title.text = card.Title;
        description.text = card.Description;
        
        Mana.text = card.Mana.ToString();
        imageSR.sprite = card.Image;
    }
    void OnMouseEnter()
{
    wrapper.SetActive(false);
    Vector3 pos = new(transform.position.x, transform.position.y+2, 0); //changed y dari -2 ke "transform.position.y+2",
    CardViewHoverSystem.Instance.Show(Card, pos);                       //klo enggak kartu tetep gede tapi masih gak keatas. bandaid fix but it works ig
}

void OnMouseExit()
{
    CardViewHoverSystem.Instance.Hide();
    wrapper.SetActive(true);
}
}