using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;

namespace FarmSimulator.Managers.TileHandler
{

    public class TileInfo
    {
        public string TileSetName{ get; private set; }
        public string ImagePath { get; private set; }
        public int firstGID { get; private set; }
        public bool isCollection { get; private set; }


        public TileInfo(int firstGID, string name, string path, bool isCollection)
        {
            this.TileSetName = name;
            this.ImagePath = path;
            this.isCollection = isCollection;
            this.firstGID = firstGID;
        }
    }

    public class TileCollectionInfo : TileInfo
    {
        public List<string> tileNames { get; private set; }

        public TileCollectionInfo(int firstGID, string name, string path, bool isCollection, List<string> tileNames) : base(firstGID, name, path, isCollection)
        {
            this.tileNames = tileNames;
        }
    }



    public class TileHandler
    {
        Dictionary<string, TileInfo> _tileDict = new Dictionary<string, TileInfo>();


        /// <summary>
        /// this function take data of the tilesets and returns the dictionary with the usable data only
        /// </summary>
        /// <param name="tileSets"></param>
        public void BuildDictionary(List<object> tileSets)
        {
            foreach (object tileSet in tileSets)
            {
                var lun = (((Dictionary<string, object>)tileSet));

                // This if else refer to if the folowing object is collection of images
                if (lun.ContainsKey("grid"))
                {
                   

                    try
                    {
                        string name = (string)lun["name"];
                        //Console.WriteLine(name);
                        List<string> names = new List<string>();

                        //Console.WriteLine(lun["firstgid"]);

                        //This block of code extract the name of the images and save it into a list so we can use it later..
                        foreach(var item in ((List<object>)lun["tiles"]))  
                        {
                            string sub = (string)((( (Dictionary<string, object>)item)["image"]));
                            string[] _name = sub.Split(name + "/");
                            //Console.WriteLine(_name[1]);
                            names.Add(_name[1]);
                        }
                       
                        TileCollectionInfo tileCollectionInfo = new TileCollectionInfo(
                            firstGID: (int)lun["firstgid"],
                            name: name,
                            path: $"Tiles/{name}/",
                            isCollection: true,
                            tileNames: names
                            );

                        _tileDict.Add(name, tileCollectionInfo);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        string sub = (string)lun["image"];

                        string[] parts = sub.Split("Assets/");
                        string name = (string)lun["name"];
                        //Console.WriteLine(parts[1]);
                        //Console.WriteLine((int)lun["firstgid"]);


                        TileInfo tileinfo = new TileInfo(
                            firstGID: (int)lun["firstgid"],
                            name: name,
                            path: parts[1],
                            isCollection: false
                            );
                        
                        _tileDict.Add(name, tileinfo);
                }
                    catch(Exception e)
                    {
                        Console.WriteLine($"Error: {e.Message}");
                    }
                }

            }


        }

        public Dictionary<string, TileInfo> getTileData()
        {
            return _tileDict;
        }

    }
}



