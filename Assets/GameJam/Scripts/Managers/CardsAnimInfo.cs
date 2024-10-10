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

        public Vector3 targetScale; // Цільовий масштаб
        public Vector3 initialScale; // Початковий масштаб

        private void Update()
        {
            // Перевіряємо, чи є торкання
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // Перевіряємо, чи це початок торкання
                if (touch.phase == TouchPhase.Began)
                {
                    isTouching = true;
                    touchTime = 0f; // Скидаємо лічильник часу
                }
                // Підтримка затискання
                else if (touch.phase == TouchPhase.Stationary && isTouching)
                {
                    touchTime += Time.deltaTime; // Додаємо час до лічильника

                    // Перевіряємо, чи затискання тривало довше за threshold
                    if (touchTime >= holdThreshold)
                    {
                        isHold = true;
                        // Додайте тут дію, яка повинна виконуватися при затисканні
                    }
                }
                // Коли торкання завершилося
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    isTouching = false;
                    isHold = false;
                    touchTime = 0f; // Скидаємо лічильник
                    elapsedTime = 0;
                }

                if (isHold && gameObject.transform.localScale.x != 0.7f)
                {
                    elapsedTime += Time.deltaTime; // Збільшуємо час, що пройшов
                    float t = elapsedTime / duration; // Нормалізуємо час
                    transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
                }
                else if (!isHold && gameObject.transform.localScale.x != 0.0f)
                {
                    elapsedTime += Time.deltaTime; // Збільшуємо час, що пройшов
                    float t = elapsedTime / duration; // Нормалізуємо час
                    transform.localScale = Vector3.Lerp(targetScale, initialScale, t);
                }

            }
        }
    }
}