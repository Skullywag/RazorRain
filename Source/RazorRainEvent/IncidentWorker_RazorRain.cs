using System;
using Verse;
namespace RimWorld
{
    public class IncidentWorker_RazorRain : IncidentWorker
    {
        public override bool TryExecute(IncidentParms parms)
        {
            IntVec3 dropCenter = CellFinderLoose.RandomCellWith((IntVec3 c) => GenGrid.Standable(c) && !Find.RoofGrid.Roofed(c) && !Find.FogGrid.IsFogged(c), 1000);
            GenSpawn.Spawn(ThingDef.Named("Thing_RazorSpawner"), dropCenter);
            Find.LetterStack.ReceiveLetter("Razor rain imminent", "Some debris in orbit around the planet has started to fall in the area, brace yourselves.", LetterType.BadUrgent);
            return true;
        }
    }
}
