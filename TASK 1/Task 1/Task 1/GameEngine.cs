﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    public class GameEngine
    {
        Map map; //filed for map
        bool gameOver = false; //set to true if game ends
        string winning = ""; //winning faction string
        int round = 0;

        public GameEngine()
        {
            map = new Map(10); //number of units
        }

        public bool GameOver
        {
            get { return gameOver; }
        }

        public string Winning
        {
            get { return winning; }
        }

        public int Round
        {
            get { return round; }
        }

        ///

        public string DisplayMap()
        {
            return map.DisplayMap(); //map. allows form to access it
        }

        public string Information()
        {
            string info = ""; //making the space between each unit
            foreach (Unit unit in map.Units)
            {
                info += unit + "\n";
            }
            return info;
        }
        ///

        public void Reset() //reset the map for a new simulation
        {
            map.Reset();
            gameOver = false;
            round = 0;
        }

        ///

        public void GameLoop()
        {
            foreach (Unit unit in map.Units) //link all units in map.Units
            {
                if (unit.IsDestroyed)
                {
                    continue; //guarding if statement: if it is destroyed, go to the next unit in the loop
                }

                Unit closest = unit.GetClosestUnit(map.Units);
                if (closest == null) //avoiding nesting more than 2 if statements (no spaghetti!)
                {
                    gameOver = true;
                    winning = unit.Faction;
                    map.UpdateDisplay();
                    return;
                }

                double percentageHealth = unit.Health / unit.MaxHealth;
                if (percentageHealth <= 0.25) //if health percentage is less than 25%, it must run randomly away
                {
                    unit.Run();
                }
                else if (unit.InRange(closest))
                {
                    unit.Combat(closest);
                }
                else
                {
                    unit.Move(closest);
                }
                MapBoundary(unit, map.Size); //if x < 0, reset x to 0 so it never leaves the map (push unit back in)
            }
        
            map.UpdateDisplay();
            round++;
        }

        ///

        private void MapBoundary(Unit unit, int size)
        {
            if (unit.X < 0) //push in x
            {
                unit.X = 0;
            }
            else if (unit.X >= size)
            {
                unit.X = size - 1;
            }
            if (unit.Y < 0) //push in y
            {
                unit.Y = 0;
            }
            else if (unit.Y >= size)
            {
                unit.Y = size - 1;
            }
        }
    }
}