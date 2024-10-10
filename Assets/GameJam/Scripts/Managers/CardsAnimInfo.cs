using UnityEngine;

namespace GameJam.Managers
{
    public class CardsAnimInfo : MonoBehaviour
    {
        [SerializeField] private float holdThreshold = 0.5f;

        [SerializeField] private float touchTime = 1f;

        private float elapsedTime = 0f;
        [SerializeField] private float duration = 1f;

        private bool isTouching = false;

        private bool isHold = false;

        public Vector3 targetScale; // ֳ������ �������
        public Vector3 initialScale; // ���������� �������

        private void Update()
        {
            // ����������, �� � ��������
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // ����������, �� �� ������� ��������
                if (touch.phase == TouchPhase.Began)
                {
                    isTouching = true;
                    touchTime = 0f; // ������� �������� ����
                }
                // ϳ������� ����������
                else if (touch.phase == TouchPhase.Stationary && isTouching)
                {
                    touchTime += Time.deltaTime; // ������ ��� �� ���������

                    // ����������, �� ���������� ������� ����� �� threshold
                    if (touchTime >= holdThreshold)
                    {
                        isHold = true;
                        // ������� ��� ��, ��� ������� ������������ ��� ���������
                    }
                }
                // ���� �������� �����������
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    isTouching = false;
                    isHold = false;
                    touchTime = 0f; // ������� ��������
                    elapsedTime = 0;
                }

                if (isHold && gameObject.transform.localScale.x != 0.7f)
                {
                    elapsedTime += Time.deltaTime; // �������� ���, �� �������
                    float t = elapsedTime / duration; // ���������� ���
                    transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
                }
                else if (!isHold && gameObject.transform.localScale.x != 0.0f)
                {
                    elapsedTime += Time.deltaTime; // �������� ���, �� �������
                    float t = elapsedTime / duration; // ���������� ���
                    transform.localScale = Vector3.Lerp(targetScale, initialScale, t);
                }

            }
        }
    }
}