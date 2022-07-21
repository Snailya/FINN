using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.AutoCAD.DatabaseServices;
using FINN.CAD.Models;

namespace FINN.CAD.Utilities
{
    internal class ContextManager
    {
        private readonly IList<BaseProxy> _proxies = new List<BaseProxy>();

        internal void Add(BaseProxy proxy)
        {
            _proxies.Add(proxy);
        }

        internal void SaveChanges()
        {
            var acDb = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager
                .MdiActiveDocument.Database;
            using (var acTrans = acDb.TransactionManager.StartTransaction())
            {
                var acBlockTable = acTrans.GetObject(acDb.BlockTableId, OpenMode.ForWrite) as BlockTable;
                var acModelSpace =
                    acTrans.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                foreach (var proxy in _proxies)
                {
                    switch (proxy)
                    {
                        case BlockProxy blockProxy:
                        {
                            var blockId = ObjectId.Null;
                            if (string.IsNullOrEmpty(blockProxy.Name))
                            {
                                blockProxy.Name = Guid.NewGuid().ToString();
                            }

                            if (acBlockTable.Has(blockProxy.Name))
                            {
                                blockId = acBlockTable[blockProxy.Name];
                            }
                            else
                            {
                                using (var acBlockTableRecord = new BlockTableRecord())
                                {
                                    acBlockTableRecord.Name = blockProxy.Name;
                                    acBlockTableRecord.Origin = blockProxy.Location;
                                    blockProxy.Id = acBlockTable.Add(acBlockTableRecord);
                                    // acTrans.AddNewlyCreatedDBObject(acBlockTable, true);

                                    foreach (Entity entity in proxy.ChildrenObjects)
                                    {
                                        var id = acBlockTableRecord.AppendEntity(entity);
                                        acTrans.AddNewlyCreatedDBObject(entity, true);
                                        proxy.Children.Add(id);
                                    }

                                    blockId = acBlockTableRecord.ObjectId;
                                }
                            }

                            using (var acBlockReference = new BlockReference(blockProxy.Location, blockId))
                            {
                                acModelSpace.AppendEntity(acBlockReference);
                                acTrans.AddNewlyCreatedDBObject(acBlockReference, true);
                            }

                            break;
                        }

                        case GroupProxy groupProxy:
                        {
                            var acGroupDictionary =
                                acTrans.GetObject(acDb.GroupDictionaryId, OpenMode.ForWrite) as DBDictionary;
                            using (var group = new Group(groupProxy.Name, true))
                            {
                                acGroupDictionary.SetAt(groupProxy.Name, group);
                                acTrans.AddNewlyCreatedDBObject(acBlockTable, true);

                                foreach (Entity entity in proxy.ChildrenObjects)
                                {
                                    var id = acModelSpace.AppendEntity(entity);
                                    acTrans.AddNewlyCreatedDBObject(entity, true);
                                    proxy.Children.Add(id);
                                }

                                group.InsertAt(0, groupProxy.Children);
                            }

                            break;
                        }
                        default:
                        {
                            foreach (Entity entity in proxy.ChildrenObjects)
                            {
                                var id = acModelSpace.AppendEntity(entity);
                                acTrans.AddNewlyCreatedDBObject(entity, true);
                                proxy.Children.Add(id);
                            }

                            break;
                        }
                    }

                    // if (proxy is IColorRegion colorRegion)
                    // {
                    //     var hatch = new Hatch();
                    //     hatch.SetHatchPattern(HatchPatternType.PreDefined, "ANSI31");
                    //     hatch.Associative = true;
                    //     hatch.AppendLoop((int)HatchLoopTypes.Default,
                    //         new ObjectIdCollection(colorRegion.Region.OfType<DBObject>().Select(d => d.ObjectId)
                    //             .ToArray()));
                    //     hatch.EvaluateHatch(true);
                    // }
                }

                acTrans.Commit();
            }
        }
    }
}