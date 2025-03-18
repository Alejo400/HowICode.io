using System;
using System.Collections;
using UnityEngine;

public class SilbonAbility : Ability<GameObject>
{
    [SerializeField] private GameObject silbonImage;
    [SerializeField] private AudioSource silbonSound;
    [SerializeField] float timeToPlaywhistle;
    [SerializeField] private float sphereRadius; // Radio de detección para OverlapSphere
    [SerializeField] private LayerMask characterLayer;
    [SerializeField] private float effectDuration;
    Character _Character;

    void Start(){
        canUseAbility = true;
        silbonSound = SoundManager._SharedInstance._PlayerSounds.silbon;
        ActionIcon = GUIManager._SharedIntance.GetActionIcon("Silbon");
    }

    public override void ExecuteAbility(GameObject _target, Character character)
    {
        _Character = character;
        // Activar la imagen del silbón
        if (silbonImage != null)
        {
            silbonImage.SetActive(true);
        }
        // Iniciar la secuencia de la habilidad
        StartCoroutine(PerformSilbonAbility());
    }
    private IEnumerator PerformSilbonAbility()
    {
        // Esperar medio segundo antes de ejecutar el sonido y la lógica
        yield return new WaitForSeconds(timeToPlaywhistle);
        // Reproducir el sonido del silbón
        if (silbonSound != null)
        {
            silbonSound.Play();
        }

        // Detectar enemigos dentro del área definida
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereRadius, characterLayer);
        // Lista de enemigos afectados
        Character[] affectedCharacters = new Character[hitColliders.Length];
        int index = 0;

        foreach (Collider collider in hitColliders)
        {
            GameObject hitObject = collider.gameObject;
            // Omitir al ejecutor de la habilidad
            if (hitObject != _Character.gameObject) {
                Enemy character = hitObject.GetComponent<Enemy>();
                if (character != null)
                {
                    // Desactivar movimiento del personaje
                    character.StateIndicator.SetActive(true);
                    character.ChangeState(new StunState());
                    affectedCharacters[index++] = character;
                }
            }
        }
        StartCoroutine(AllowUseAbility());

        // Esperar la duración del efecto
        yield return new WaitForSeconds(effectDuration);
        // Restaurar la capacidad de moverse
        foreach (Enemy character in affectedCharacters)
        {
            if (character != null)
            {
                character.ChangeState(new ChaseState());
                character.StateIndicator.SetActive(false);
            }
        }
        // Desactivar la imagen del silbón
        if (silbonImage != null)
        {
            silbonImage.SetActive(false);
        }
    }
    public override IEnumerator AllowUseAbility()
    {
        canUseAbility = false;
        yield return new WaitForSeconds(cooldown);
        canUseAbility = true;
    }
}
