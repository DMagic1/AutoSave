/* DMagic Orbital Science - AutoSave
 * Creates a backup save file upon initial game load
 *
 * Copyright (c) 2014, DMagic
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 * 
 * 1. Redistributions of source code must retain the above copyright notice, 
 * this list of conditions and the following disclaimer.
 * 
 * 2. Redistributions in binary form must reproduce the above copyright notice, 
 * this list of conditions and the following disclaimer in the documentation and/or other materials 
 * provided with the distribution.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
 * GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT 
 * OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *  
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace AutoSave
{
    [KSPAddonImproved(KSPAddonImproved.Startup.SpaceCenter | KSPAddonImproved.Startup.Flight | KSPAddonImproved.Startup.EditorAny | KSPAddonImproved.Startup.TrackingStation | KSPAddonImproved.Startup.MainMenu, false)]
    public class AutoSave : MonoBehaviour
    {
        private int max = 3;
        private ConfigNode node = null;
        private string path = null;
        private static bool Saved;

        public void Start()
        {
            if (HighLogic.LoadedScene == GameScenes.MAINMENU)
                Saved = false;
            else if (Saved == false)
                saveBackup();
        }

        public void saveBackup()
        {
            DateTime oldestFile = new DateTime(2050, 1, 1);
            string replaceBackup = null;
            string activeDirectory = Path.Combine(Path.Combine(new DirectoryInfo(KSPUtil.ApplicationRootPath).FullName, "saves"), HighLogic.fetch.GameSaveFolder);
            path = Path.Combine(new DirectoryInfo(KSPUtil.ApplicationRootPath).FullName, "GameData/AutoSave/Settings.cfg").Replace("\\", "/");

            if (File.Exists(path))       //Load Settings.cfg to check for change in max number of saves
            {
                max = getMaxSave("MaxSaves");
                print("Changing max saves value to " + max.ToString());
            }

            for (int i = 0; i < max; i++)
            {
                string filepath = Path.Combine(activeDirectory, "persistent Backup " + i.ToString() + ".sfs");
                if (!File.Exists(filepath))
                {
                    replaceBackup = "persistent Backup " + i.ToString() + ".sfs";
                    break;
                }
                else                   //If all backups have been written, check for the oldest file and rewrite that one
                {
                    DateTime modified = File.GetLastWriteTime(filepath);
                    if (modified < oldestFile)
                    {
                        replaceBackup = "persistent Backup " + i.ToString() + ".sfs";
                        oldestFile = modified;
                    }
                }
            }
            File.Copy(Path.Combine(activeDirectory, "persistent.sfs"), Path.Combine(activeDirectory, replaceBackup), true);
            print("Backup saved as " + replaceBackup);
            Saved = true;
        }

        public int getMaxSave(string entry) //Make sure that no amount of screwing up the Settings file will break the plugin
        {
            int number = 3;
            node = ConfigNode.Load(path);
            if (node != null)
            {                
                string value = node.GetValue(entry);
                if (value == null) return number;
                else if (Int32.TryParse(value, out number))
                    return number;
                else return 3;
            }
            else return number;
        }
    }
}
