using Maze.Library;
using System.Collections.Generic;
using System.Drawing;

namespace Maze.Solver
{
    /// <summary>
    /// Moves a robot from its current position towards the exit of the maze
    /// </summary>
    public class RobotController
    {
        private IRobot robot;
        private bool end;
        private HashSet<Point> points;

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotController"/> class
        /// </summary>
        /// <param name="robot">Robot that is controlled</param>
        public RobotController(IRobot robot)
        {
            // Store robot for later use
            this.robot = robot;
            this.end = false;
            this.points = new HashSet<Point>();
        }

        /// <summary>
        /// Moves the robot to the exit
        /// </summary>
        /// <remarks>
        /// This function uses methods of the robot that was passed into this class'
        /// constructor. It has to move the robot until the robot's event
        /// <see cref="IRobot.ReachedExit"/> is fired. If the algorithm finds out that
        /// the exit is not reachable, it has to call <see cref="IRobot.HaltAndCatchFire"/>
        /// and exit.
        /// </remarks>
        public void MoveRobotToExit()
        {
            robot.ReachedExit += (_, __) => this.end = true;

            this.Exit(0, 0);

            // Tip: Consider writing `if (!reachedEnd)` instead of `... == false`
            if (this.end == false)
            {
                this.robot.HaltAndCatchFire();
            }
        }
        private void Exit(int x, int y)
        {
            if (this.end == false && this.points.Add(new Point(x, y)))
            {
                //Right
                bool status = this.robot.CanIMove(Direction.Right);
                if (status && this.end == false)
                {
                    this.robot.Move(Direction.Right);
                    this.Exit(x + 1, y);
                    if (this.end == false)
                    {
                        this.robot.Move(Direction.Left);
                    }
                }

                //Down
                status = this.robot.CanIMove(Direction.Down);
                if (status && this.end == false)
                {
                    this.robot.Move(Direction.Down);
                    this.Exit(x, y + 1);
                    if (this.end == false)
                    {
                        this.robot.Move(Direction.Up);
                    }
                }

                //Left
                status = this.robot.CanIMove(Direction.Left);
                if (status && this.end == false)
                {
                    this.robot.Move(Direction.Left);
                    this.Exit(x - 1, y);
                    if (this.end == false)
                    {
                        this.robot.Move(Direction.Right);
                    }
                }

                //Up
                status = this.robot.CanIMove(Direction.Up);
                if (status && this.end == false)
                {
                    this.robot.Move(Direction.Up);
                    this.Exit(x, y - 1);
                    if (this.end == false)
                    {
                        this.robot.Move(Direction.Down);
                    }
                }
            }
        }
    }
}
