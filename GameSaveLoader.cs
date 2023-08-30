//using System;
//using System.IO;
//using System.Collections.Generic;
//using Newtonsoft.Json; // Newtonsoft.Json 라이브러리를 추가해야 합니다.
//using System.Formats.Asn1;
//using System.Numerics;
//using System.Text.Json;

//namespace SpartaDungeonBattle
//{
//    public class GameSaveLoader
//    {
//        static JsonSerializer serializer = new JsonSerializer(); // JsonSerializer를 준비합니다.

//        static void GameDataLoad()
//        {
//            if (File.Exists("Player.json"))
//            {
//                using (StreamReader streamReader = new StreamReader("player.json"))
//                using (JsonReader jsonReader = new JsonTextReader(streamReader))
//                {
//                    player = serializer.Deserialize<Character>(jsonReader); // JSON을 역직렬화합니다.
//                }
//            }

//            static void GameDataSave()
//            {
//                using (StreamWriter streamWriter = new StreamWriter("player.json"))
//                using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
//                {
//                    serializer.Serialize(jsonWriter, player); // JSON으로 직렬화합니다.
//                }
//            }
//        }
//    }
//}
