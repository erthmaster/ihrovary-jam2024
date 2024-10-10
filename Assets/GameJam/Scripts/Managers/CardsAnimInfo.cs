using UnityEngine;

namespace GameJam.Managers
{
    public class CardsAnimInfo : MonoBehaviour
    {
        private Animator animator; // ��������� Animator
        void Update()
        {
            // �������� ������
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // �������� raycast ��� ����������, �� ��������� ������
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform) // ���� ���������� ���� ������
                    {
                        if (touch.phase == TouchPhase.Began)
                        {
                            animator.SetBool("isHolding", true); // �������� �������
                        }
                        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        {
                            animator.SetBool("isHolding", false); // �������� �������
                        }
                    }
                }
            }
        }
    }
}