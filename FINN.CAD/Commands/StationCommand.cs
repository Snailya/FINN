using System.Collections.Generic;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using FINN.COMMON.Dtos;
using Newtonsoft.Json;
using FINN.COMMON.Constants;
using FINN.CAD.Models;
using FINN.CAD.Utilities;
using System.IO;

[assembly: CommandClass(typeof(FINN.CAD.Commands.StationCommand))]

namespace FINN.CAD.Commands
{
    public class StationCommand
    {
        private const double Gutter = 10;

        /// <summary>
        /// Convert command parameter into structured DTO.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static T GetParameters<T>()
        {
            var acDoc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            var filePathOpt = new PromptStringOptions("");
            var filePathResult = acDoc.Editor.GetString(filePathOpt);

            var filePath = filePathResult.StringResult.FromBase64();
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
        }

        /// <summary>
        /// Create a station block from object file.
        /// </summary>
        [CommandMethod("FINN", Command.CREATE_STATION_SILENT, CommandFlags.Modal)]
        public void CreateStationSilent()
        {
            var dto = GetParameters<StationDto>();
            var location = new Point3d(0, 0, 0);

            var cm = new ContextManager();
            var station = new Station(location, dto.Title, dto.Body);
            cm.Add(station);
            cm.SaveChanges();
        }

        /// <summary>
        /// Create a station group from object file.
        /// </summary>
        [CommandMethod("FINN", Command.CREATE_STATION_GROUP_SILENT, CommandFlags.Modal)]
        public void CreateStationInGroupSilent()
        {
            var dto = GetParameters<StationGroupDto>();
            var location = new Point3d(0, 0, 0);

            var cm = new ContextManager();
            var stations = new List<Station>();
            foreach (var stationDto in dto.Stations)
            {
                var station = new Station(location, stationDto.Title, stationDto.Body);
                stations.Add(station);
                cm.Add(station);

                location = new Point3d(station.GeoExtents.MaxPoint.X + Gutter, location.Y, 0);
            }

            var fence = new Fence(stations, dto.Name);
            cm.Add(fence);

            cm.SaveChanges();
        }

        /// <summary>
        /// Create a station line from object file.
        /// </summary>
        [CommandMethod("FINN", Command.CREATE_STATION_LINE_SILENT, CommandFlags.Modal)]
        public void CreateStationInLineSilent()
        {
            var dto = GetParameters<StationLineDto>();
            var location = new Point3d(0, 0, 0);

            var cm = new ContextManager();
            foreach (var groupDto in dto.Groups)
            {
                var stations = new List<Station>();
                foreach (var stationDto in groupDto.Stations)
                {
                    var station = new Station(location, stationDto.Title, stationDto.Body);
                    stations.Add(station);
                    cm.Add(station);

                    location = new Point3d(station.GeoExtents.MaxPoint.X + Gutter, location.Y, 0);
                }

                var fence = new Fence(stations, groupDto.Name);
                cm.Add(fence);

                location = new Point3d(fence.MinPoint.X, fence.MinPoint.Y - Gutter, 0);
            }

            cm.SaveChanges();
        }
    }
}