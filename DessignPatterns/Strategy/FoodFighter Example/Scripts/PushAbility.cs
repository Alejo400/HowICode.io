using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
public class PushAbility : Ability<GameObject>
{
    [SerializeField] private float pushForce; // Fuerza del empuje
    [SerializeField] private Transform raycastOrigin; // Punto de origen del Raycast
    [SerializeField] private float raycastRange; // Distancia máxima del Raycast
    public float RayCastRange { get => raycastRange; set => raycastRange = value; }
    [SerializeField] private float raycastRadius; // Distancia máxima del Raycast
    public float RaycastRadius { get => raycastRadius; set => raycastRadius = value; }
    private float initialRaycastRange; // Valor inicial de raycastRange
    [SerializeField] private LayerMask CharacterLayer; // Capa de los enemigos
    [SerializeField] private float pushDuration;
    [SerializeField] private GameObject PushParticle; // Sistema de partículas para el golpe
    [SerializeField] ParticleSystem[] particleSystems;
    //Variables para la bola de energia
    [SerializeField] GameObject EnergyBall;
    ParticleSystem EnergyBallParticle;
    bool doingSuperBall; // Si estamos lanzando el ataque cargado
    public bool DoingSuperBall { get => doingSuperBall; set => doingSuperBall = value; }
    [SerializeField] float energyBallDuration;
    [SerializeField] float speedEnergyBall;
    [SerializeField] ChargeAbility SuperPushAbility; //Carga de golpe mas poderoso
    [SerializeField] GameObject RefToReposEnergyBall;
    void Start(){
        canUseAbility = true;
        EnergyBall = GameObject.Find("EnergyBall");
        SaveParticles();
        initialRaycastRange = raycastRange;
        if(EnergyBall != null){
            EnergyBallParticle = EnergyBall.GetComponentInChildren<ParticleSystem>();
        }
        //Obtener el icono de la habilidad
        ActionIcon = GUIManager._SharedIntance.GetActionIcon("Push");
    }
    /// <summary>
    /// Ejecutamos la habilidad
    /// </summary>
    /// <param name="_target">Objetivo</param>
    /// <param name="character">Quien realiza la habilidad</param>
    public override void ExecuteAbility(GameObject _target, Character character)
    {
        Debug.Log("PushAbility");
        raycastOrigin = character.DetectRival; // Punto desde donde lanzamos el Raycast
        StartCoroutine(TryPush(character.gameObject.transform));
        if(doingSuperBall)
            StartCoroutine(CallEnergyBall());
    }
    /// <summary>
    /// Empujar a un personaje de layer character hacia adelante
    /// </summary>
    /// <param name="_transform">transform del character que realiza la habilidad</param>
    private IEnumerator TryPush(Transform _transform)
    {
        RaycastHit[] hits = Physics.SphereCastAll(raycastOrigin.position, raycastRadius, 
                                              _transform.forward, raycastRange, CharacterLayer);

        foreach (RaycastHit hit in hits)
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject != _transform.gameObject) // Ignorar al propio jugador
            {
                Rigidbody enemyRb = hit.collider.GetComponent<Rigidbody>();

                if (enemyRb != null)
                {
                    Character enemyCharacter = hit.collider.GetComponent<Character>();
                    if (enemyCharacter != null)
                    {
                        enemyCharacter.Hit();
                        if(!doingSuperBall)
                            ReposPushParticles(hit);
                    }

                    // Determinar dirección de empuje basada en la posición relativa
                    Vector3 pushDirection = _transform.forward; // La dirección del Raycast ya es hacia adelante
                    pushDirection = new Vector3(pushDirection.x, 0, 0); // Solo en X
                    // Iniciar el empuje constante
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    StartCoroutine(ApplyPushForce(enemyRb, pushDirection, enemy));
                    //Volver el raycastrange a su valor inicial
                    raycastRange = initialRaycastRange;
                }
            }
        }
        //Comprobamos la carga pocos segundos despues que la habilidad ha sido llamada
        yield return new WaitForSeconds(0.1f);
        //CheckChargingSuperPush();
    }
    /// <summary>
    /// Activar la bola de energia e impulsarla hacia adelante
    /// </summary>
    /// <returns></returns>
    IEnumerator CallEnergyBall(){
        //Reposicionar bola de energia
        EnergyBall.transform.position = new Vector3(RefToReposEnergyBall.transform.localPosition.x,
                                                    RefToReposEnergyBall.transform.localPosition.y + 0.5f,
                                                    RefToReposEnergyBall.transform.localPosition.z);
        EnergyBall.transform.rotation = RefToReposEnergyBall.transform.localRotation;
        yield return new WaitForSeconds(0.1f);
        //Activar y ponerle en movimiento
        EnergyBall.SetActive(true);
        EnergyBallParticle.Play();
        float elapsedTime = 0f;
        doingSuperBall = false;

        while (elapsedTime < energyBallDuration)
        {
            EnergyBall.transform.Translate(Vector3.forward * speedEnergyBall * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //Finalizar bola de energia
        EnergyBallParticle.Stop();
        EnergyBall.SetActive(false);
    }
    /// <summary>
    /// Aplicar una fuerza constante en el tiempo a las victimas
    /// </summary>
    /// <param name="enemyRb">rigibody de las victimas</param>
    /// <param name="pushDirection">direccion hacia la cual empujamos</param>
    /// <returns></returns>
    private IEnumerator ApplyPushForce(Rigidbody enemyRb, Vector3 pushDirection, Enemy enemy)
    {
        if(enemy != null){
            enemy.Hitted = true;
        }

        pushDirection = pushDirection.normalized;
        float elapsedTime = 0f;

        while (elapsedTime < pushDuration)
        {
            enemyRb.AddForce(pushDirection * pushForce, ForceMode.Force);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        if(enemy != null){
            enemy.Hitted = false;
        }
        // Detener el movimiento después de que pase el tiempo
        enemyRb.linearVelocity = Vector3.zero;
        //Iniciar cooldown de la habilidad
        StartCoroutine(AllowUseAbility());
    }
    /// <summary>
    /// Habilitar o desahabilitar la habilidad basado en el cooldown
    /// </summary>
    /// <returns></returns>
    public override IEnumerator AllowUseAbility()
    {
        Debug.Log("Cooldown: " + cooldown);
        canUseAbility = false;
        yield return new WaitForSeconds(cooldown);
        canUseAbility = true;
    }
    void SaveParticles(){
        if(PushParticle != null){
            particleSystems = PushParticle.GetComponentsInChildren<ParticleSystem>();
            PushParticle.SetActive(false);
        }
    }
    /// <summary>
    /// Posicionar las partículas en el enemigo
    /// </summary>
    void ReposPushParticles(RaycastHit hit){
        if (PushParticle != null)
        {
            PushParticle.transform.position = hit.collider.transform.position;
            PushParticle.transform.rotation = Quaternion.identity;
            PushParticle.gameObject.SetActive(true);
            foreach (var particle in particleSystems)
            {
                particle.Play();

            }
            StartCoroutine(DesactivateParticles());
        }
    }
    private IEnumerator DesactivateParticles()
    {
        yield return new WaitForSeconds(2);
        PushParticle.SetActive(false);
    }
    /// <summary>
    /// Chequear si el jugador esta buscando cargar el super golpe
    /// </summary>
    private void CheckChargingSuperPush(){
        if(Input.GetKey(KeyCode.Return)){
            if(SuperPushAbility != null)
                SuperPushAbility.ExecuteAbility();
        }
    }
}
