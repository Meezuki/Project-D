using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text Mana;
    [SerializeField] private SpriteRenderer imageSR;
    [SerializeField] private GameObject wrapper;
    public Card Card { get; private set; } //reference card for later
    private Vector3 dragStartPosition;
    private  Quaternion dragStartRotation;
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
        if(!Interactions.Instance.PlayerCanHover()) return;
        wrapper.SetActive(false);
        Vector3 pos = new(transform.position.x, transform.position.y+2, 0);
        // Vector3 pos = new(transform.position.x, -2, 0); //changed y dari -2 ke "transform.position.y+2",
        CardViewHoverSystem.Instance.Show(Card, pos);                       //klo enggak kartu tetep gede tapi masih gak keatas. bandaid fix but it works ig
    }
    void OnMouseExit()
    {
        CardViewHoverSystem.Instance.Hide();
        wrapper.SetActive(true);
    }
    void OnMouseDown()
    {
        if(!Interactions.Instance.PlayerCanInteract()) return;
        Interactions.Instance.PlayerIsDragging = true;  
        wrapper.SetActive(true);
        CardViewHoverSystem.Instance.Hide();
        dragStartPosition = transform.position;
        dragStartRotation = transform.rotation;
        transform.rotation = quaternion.Euler(0, 0, 0);
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
    }

        void OnMouseDrag()
    {
        if(!Interactions.Instance.PlayerCanInteract()) return;  
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
    }

        void OnMouseUp()
    {
        if(!Interactions.Instance.PlayerCanInteract()) return;
        if(Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f))
        {
            PlayCardGA playCardGA = new(Card);
            ActionSystem.Instance.Perform(playCardGA);
        }
        else
        {
            transform.position = dragStartPosition;
            transform.rotation = dragStartRotation; 
        }
        Interactions.Instance.PlayerIsDragging = false;
    }
}