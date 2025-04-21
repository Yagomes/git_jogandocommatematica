using System.Collections;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            // Verifica se a inst�ncia j� foi criada
            if (instance == null)
            {
                // Tenta encontrar uma inst�ncia existente na cena
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    // Se n�o houver nenhuma inst�ncia, loga um erro
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        // Se j� existir uma inst�ncia e n�o for o objeto atual, destrua a nova inst�ncia
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // Caso contr�rio, atribui a inst�ncia do objeto atual
            instance = this as T;
            // Faz o objeto persistir entre cenas
            DontDestroyOnLoad(gameObject);
        }
    }
}
