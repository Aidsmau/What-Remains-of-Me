using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JonesOutlet : AControllable
{
    [SerializeField] private SpriteRenderer door;
    [SerializeField] private Sprite openDoorSprite;
    [SerializeField] private Collider2D doorCollider;
    [SerializeField] private Animator doorAnimator;

    [SerializeField] private SpriteRenderer door2;
    [SerializeField] private Sprite openDoorSprite2;
    [SerializeField] private Collider2D doorCollider2;
    [SerializeField] private Animator doorAnimator2;
    
    [SerializeField] private SpriteRenderer door3;
    [SerializeField] private Sprite openDoorSprite3;
    [SerializeField] private Collider2D doorCollider3;
    [SerializeField] private Animator doorAnimator3;

    [SerializeField] private Slider slider;

    [SerializeField] private ParticleSystem explosionParticles;
    private void Update()
    {
        slider.value = GetVirus() / 100f;

        if (GetVirus() >= 80f)
        {
            explosionParticles.Play();
            door.sprite = openDoorSprite;
            doorAnimator.enabled = false;
            doorCollider.enabled = false;

	        door2.sprite = openDoorSprite2;
            doorAnimator2.enabled = false;
            doorCollider2.enabled = false;

	        door3.sprite = openDoorSprite3;
            doorAnimator3.enabled = false;
            doorCollider3.enabled = false;
        }
    }
}
