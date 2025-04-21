using System.Collections;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            // Verifica se a instância já foi criada
            if (instance == null)
            {
                // Tenta encontrar uma instância existente na cena
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    // Se não houver nenhuma instância, loga um erro
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        // Se já existir uma instância e não for o objeto atual, destrua a nova instância
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // Caso contrário, atribui a instância do objeto atual
            instance = this as T;
            // Faz o objeto persistir entre cenas
            DontDestroyOnLoad(gameObject);
        }
    }
}
