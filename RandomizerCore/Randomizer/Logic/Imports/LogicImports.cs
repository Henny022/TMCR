﻿using RandomizerCore.Core;
using RandomizerCore.Randomizer.Enumerables;
using RandomizerCore.Randomizer.Logic.Defines;
using RandomizerCore.Randomizer.Logic.Dependency;
using RandomizerCore.Randomizer.Models;

namespace RandomizerCore.Randomizer.Logic.Imports;

/*
 * This class defines functions that can be imported with the !import statement in logic.
 * Functions should all fit the following template:
 *   private static bool [DefineName]Import(Location.Location self, Item itemToPlace, List<Item> availableItems, List<Location.Location> allLocations)
 *
 * The list of functions, Dictionary<string, Func<Location.Location, Item, List<Item>, List<Location.Location>, bool>> FunctionValues, should have the string be the name of the imported
 * define, and the Func will be a lambda that calls into the function you wrote.
 *
 * Functions should return true if the condition is met (the item can be placed on the target location), and false if not
 */
public static class LogicImports
{
    public static Dictionary<string, Func<Location.Location, Item, List<Item>, List<Location.Location>, bool>> FunctionValues = new()
    {
        {
            "NON_ELEMENT_DUNGEONS_BARREN",
            NonElementDungeonsBarrenImport
        },
        {
            "NON_ELEMENT_DUNGEONS_NOT_REQUIRED",
            NonElementDungeonsNotRequiredImport
        },
    };

    private static bool NonElementDungeonsBarrenImport(Location.Location self, Item itemToPlace, List<Item> availableItems, List<Location.Location> allLocations)
    {
        if (itemToPlace.ShufflePool is not ItemPool.Major || self.Dungeons.Count == 0 || self.Dungeons.All(dungeon => dungeon != itemToPlace.Dungeon)) return true;

        var prizeDungeonForItem = allLocations.First(prize => prize.Dungeons.Any(dungeon => dungeon == itemToPlace.Dungeon));
        
        return prizeDungeonForItem.Contents is { Type: ItemType.EarthElement or ItemType.FireElement or ItemType.WaterElement or ItemType.WindElement };
    }
    
    private static bool NonElementDungeonsNotRequiredImport(Location.Location self, Item itemToPlace, List<Item> availableItems, List<Location.Location> allLocations)
    {
        const string beatVaatiDefineName = "BeatVaati";
        if (itemToPlace.ShufflePool is not ItemPool.Major || self.Dungeons.Count == 0 || self.Dungeons.All(dungeon => dungeon != itemToPlace.Dungeon)) return true;

        var prizeDungeonForItem = allLocations.First(prize => prize.Dungeons.Any(dungeon => dungeon == itemToPlace.Dungeon));

        return prizeDungeonForItem.Contents is
        {
            Type: ItemType.EarthElement or ItemType.FireElement or ItemType.WaterElement or ItemType.WindElement
        } || new LocationDependency(beatVaatiDefineName).DependencyFulfilled(availableItems, allLocations);
    }
}