﻿using System;
using System.Diagnostics;
using System.IO;

namespace Nett.Coma.Tests
{
    public abstract class TestsBase
    {
        protected static void ModifyFileOnDisk(string fileName, Action<SingleLevelConfig> modify)
        {
            var read = Toml.ReadFile<SingleLevelConfig>(fileName);
            modify(read);
            Toml.WriteFile(read, fileName);
        }

        protected static void TryDeleteFile(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine("Failed to cleanup file:" + exc.ToString());
            }
        }

        protected static void TryDeleteAllTomlFiles()
        {
            try
            {
                var files = Directory.GetFiles(".", $"*{Toml.FileExtension}");
                foreach (var f in files)
                {
                    File.Delete(f);
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine("Failed to delete TOML files: " + exc.ToString());
            }
        }
    }
}