using UnityEngine;

public class TestSystem : MonoBehaviour
{
    [SerializeField] private HandView handView;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CardView cardView = CardViewCreator.Instance.CreateCardView(transform.position, Quaternion.identity);
            StartCoroutine(handView.AddCard(cardView));
        }
    }
}