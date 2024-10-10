using UnityEngine;

namespace GameJam.Managers
{
    public class CardsAnimInfo : MonoBehaviour
    {
        private Animator animator; // Компонент Animator
        void Update()
        {
            // Перевірка дотиків
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // Виконуємо raycast для визначення, чи торкаємося картки
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform) // Якщо торкнулися цієї картки
                    {
                        if (touch.phase == TouchPhase.Began)
                        {
                            animator.SetBool("isHolding", true); // Включаємо анімацію
                        }
                        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        {
                            animator.SetBool("isHolding", false); // Вимикаємо анімацію
                        }
                    }
                }
            }
        }
    }
}