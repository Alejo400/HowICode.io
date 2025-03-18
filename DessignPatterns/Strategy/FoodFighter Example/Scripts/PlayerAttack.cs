using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] protected List<Ability<GameObject>> Abilities 
                                = new List<Ability<GameObject>>();
    Dictionary<string,Ability<GameObject>> DictionaryAbilities 
                                = new Dictionary<string, Ability<GameObject>>();
    Dictionary<string,bool> AbilitiesUse
                                = new Dictionary<string,bool>();
    Animator animator;
    Rigidbody rb;
    Player _player;
    bool iAttacking;
    public bool IAttacking { get => iAttacking; }
    void Awake(){
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        _player = GetComponent<Player>();
    }
    void Start(){
        SaveAbilitiesOnDictionary();
    }
    void SaveAbilitiesOnDictionary(){
        foreach (var item in Abilities)
        {
            DictionaryAbilities.Add(item.AbilityName,item);
        }
        foreach (var item in Abilities)
        {
            AbilitiesUse.Add(item.AbilityName,true);
        }
    }
    /// <summary>
    /// Llamar a la habilidad a realizar
    /// </summary>
    /// <param name="idName"></param>
    /// <param name="soundAbility"></param>
    /// <param name="AnimName">Animacion del personaje al usar esa habilidad</param>
    public void CallAbility(string idName, AudioSource soundAbility, string animName, float divideTimeAnim){
        if(DictionaryAbilities[idName].CanUseAbility){
            animator.SetTrigger(animName);
            iAttacking = true;
            //if(_player.IsOnGround) PARA EVITAR QUE SE PARALICE EN EL AIRE AL ATACAR
            StartCoroutine(StopPlayer(idName));
            StartCoroutine(PerformAbility(idName,soundAbility,divideTimeAnim));
        }else
        {
            Debug.Log("La habilidad aun no puede usarse");
        }
    }
    /// <summary>
    /// Realizar la logica de habilidad tras cierto tiempo de haberse ejecutado su animacion
    /// </summary>
    /// <param name="id"></param>
    /// <param name="soundAbility"></param>
    /// <param name="divideSound">Dividir el tiempo de espera de la animacion</param>
    /// <returns></returns>
    IEnumerator PerformAbility(string idName, AudioSource soundAbility, float divideTimeAnim){
        float adjustedDuration = DictionaryAbilities[idName].AnimDuration.length 
                                / animator.GetCurrentAnimatorStateInfo(0).speed;
        yield return new WaitForSeconds(adjustedDuration/divideTimeAnim);
        //Deshabilitar indicador UI skill
        GUIManager._SharedIntance.DisableActionIcon(DictionaryAbilities[idName].ActionIcon,
                                                    DictionaryAbilities[idName].cooldown);
        SoundManager._SharedInstance.PlaySound(soundAbility,true);
        DictionaryAbilities[idName].ExecuteAbility(null,GetComponent<Character>());
    }
    IEnumerator StopPlayer(string idName){
        _player.CanMove = false;
        animator.SetBool("isWalking",false);
        yield return new WaitForSeconds(DictionaryAbilities[idName].freezeCooldown);
        // Reseteo de inputs o acumulación
        rb.linearVelocity = Vector3.zero;
        _player.CanMove = true;
        iAttacking = false;
    }
}
