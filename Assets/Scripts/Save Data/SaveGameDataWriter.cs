using System;
using System.IO;
using UnityEngine;

namespace SG
{
    public class SaveGameDataWriter
    {
        public string saveDataDirectoryPath = "";
        public string dataSaveFileName = "";

        public CharacterSaveData LoadCharacterDataFromJson()
        {
            string savePath = Path.Combine(saveDataDirectoryPath, dataSaveFileName);

            CharacterSaveData loadedSaveData = null;

            if (File.Exists(savePath))
            {
                try
                {
                    string saveDataToLoad = "";

                    using (FileStream stream = new FileStream(savePath, FileMode.Open))
                    {
                        using (StreamReader reder = new StreamReader(stream))
                        {
                            saveDataToLoad = reder.ReadToEnd();
                        }
                    }

                    // Deserialize data
                    loadedSaveData = JsonUtility.FromJson<CharacterSaveData>(saveDataToLoad);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(ex.Message);
                }
            }
            else
            {
                Debug.Log("SAVE FILE DOES NOT EXIST");
            }

            return loadedSaveData;
        }

        public void WriteCharacterDataToSaveFile(CharacterSaveData characterData)
        {
            //  Creates a path to save our file
            string savePath = Path.Combine(saveDataDirectoryPath, dataSaveFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                //Debug.Log("SAVE PATH = " + savePath);

                // Serialize the C# game data object to json
                string dataToStore = JsonUtility.ToJson(characterData, true);

                //write the file to our system
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("ERROR WHILE TRYING TO SAVE DATA, GAME COULD NOT BE SAVED" + ex);
            }
        }

        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, dataSaveFileName));
        }

        public bool CheckIfSaveFileExists()
        {
            if (File.Exists(Path.Combine(saveDataDirectoryPath, dataSaveFileName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}