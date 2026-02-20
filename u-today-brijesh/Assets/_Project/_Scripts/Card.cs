using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private Button button;

    private CardImageDatabase.ItemData imageDataSO = null;
    private int cardId;

    private bool flipped;
    private bool turning;

    private void OnEnable() => button.onClick.AddListener(OnClickedCardButton);

    private void OnDisable() => button.onClick.RemoveListener(OnClickedCardButton);

    private IEnumerator Flip90(Transform thisTransform, float time, bool changeSprite)
    {
        Quaternion startRotation = thisTransform.rotation;
        Quaternion endRotation = thisTransform.rotation * Quaternion.Euler(new Vector3(0, 90, 0));
        float rate = 1.0f / time;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;
            thisTransform.rotation = Quaternion.Slerp(startRotation, endRotation, t);

            yield return null;
        }
        if (changeSprite)
        {
            flipped = !flipped;
            ChangeSprite();
            StartCoroutine(Flip90(transform, time, false));
        }
        else
            turning = false;
    }

    public void Flip()
    {
        turning = true;
        StartCoroutine(Flip90(transform, 0.25f, true));
    }

    private void ChangeSprite()
    {
        if (imageDataSO == null || img == null) return;
        if (flipped)
            img.sprite = imageDataSO.Sprite;
        else
            img.sprite = GameManager.Instance.CardBack();
    }

    public void Inactive() => StartCoroutine(Fade());

    private IEnumerator Fade()
    {
        float rate = 1.0f / 2.5f;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;
            img.color = Color.Lerp(img.color, Color.clear, t);

            yield return null;
        }
    }

    public void Active()
    {
        if (img) img.color = Color.white;
    }

    public CardImageDatabase.ItemData ImageDataSO
    {
        set
        {
            imageDataSO = value;
            flipped = true;
            ChangeSprite();
        }
        get { return imageDataSO; }
    }

    public int CardId
    {
        set { cardId = value; }
        get { return cardId; }
    }
    public void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        flipped = true;
    }

    public void OnClickedCardButton()
    {
        if (flipped || turning || imageDataSO == null) return;
        if (!GameManager.Instance.CanCardClick()) return;
        AudioHandler.Instance.PlayAudioClip(AudioHandler.Instance.clipCardFlip);

        Flip();
        StartCoroutine(SelectionEvent());
    }

    private IEnumerator SelectionEvent()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.OnClickedCardButton(imageDataSO.Id, cardId);
    }
}
