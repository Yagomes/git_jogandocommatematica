using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnableTrue : MonoBehaviour // Ativa ou desativa Buttons com base em condições específicas.
{
    private void Update()
    {

    // Procura todos os GameObjects na cena, incluindo DontDestroyOnLoad
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Verifica se o nome é "Active Inventory"
            if (obj.name == "IsSoma")
            {

                foreach (GameObject obj_of in allObjects)
                {
                    // Verifica se o nome é "Active Inventory"
                    if (obj_of.name == "tela_m")
                    {

                        obj_of.SetActive(false);

                        foreach (GameObject obj_ot in allObjects)
                        {
                            if (obj_ot.name == "tela_s") 
                            {
                                obj_ot.SetActive(true);
                                break; // Interrompe o loop após encontrar o objeto
                             
                            }
                        }


                            
                    }
                }

            }
             if (obj.name == "IsMult")
            {

                foreach (GameObject obj_of in allObjects)
                {
                    // Verifica se o nome é "Active Inventory"
                    if (obj_of.name == "tela_s")
                    {

                        obj_of.SetActive(false);

                        foreach (GameObject obj_ot in allObjects)
                        {
                            if (obj_ot.name == "tela_m")
                            {
                                obj_ot.SetActive(true);
                                break; // Interrompe o loop após encontrar o objeto

                            }
                        }



                    }
                }

            }

        }
    }
}



