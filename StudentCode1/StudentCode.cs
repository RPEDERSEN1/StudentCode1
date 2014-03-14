// <copyright file="StudentCode.cs" company="Pioneers in Engineering">
// Licensed to Pioneers in Engineering under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  Pioneers in Engineering licenses 
// this file to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
//  with the License.  You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.
// </copyright>

namespace StudentPiER
{
    using System;
    using PiE.HAL.GHIElectronics.NETMF.FEZ;
    using PiE.HAL.GHIElectronics.NETMF.Hardware;
    using PiE.HAL.Microsoft.SPOT;
    using PiE.HAL.Microsoft.SPOT.Hardware;
    using PiEAPI;

    /// <summary>
    /// Student Code template
    /// </summary>
    public class StudentCode : RobotCode
    {
        /// <summary>
        /// This is your robot
        /// </summary>
        private Robot robot;

        /// <summary>
        /// This stopwatch measures time, in seconds
        /// </summary>
        private Stopwatch stopwatch;

        /// <summary>
        /// The right drive motor, on connector M0
        /// </summary>
        private GrizzlyBear rightMotor;

        /// <summary>
        /// The left drive motor, on connector M1
        /// </summary>
        private GrizzlyBear leftMotor;

        /// <summary>
        ///The encoder connected to the left motor.
        /// </summary>
        private GrizzlyEncoder leftEncoder;

        /// <summary>
        /// The encoder connected to the right motor.
        /// </summary>
        private GrizzlyEncoder rightEncoder;


        /// <summary>
        /// The sonar sensor on connector A5
        /// </summary>
        private AnalogSonarDistanceSensor sonar;

        /// <summary>
        /// A flag to toggle RFID usage in the code
        /// </summary>
        private bool useRfid;

        /// <summary>
        /// The rfid sensor
        /// </summary>
        private Rfid rfid;

        /// <summary>
        ///   Initializes a new instance of the
        ///   <see cref="StudentPiER.StudentCode"/> class.
        /// </summary>
        /// <param name='robot'>
        ///   The Robot to associate with this StudentCode
        /// </param>
        public StudentCode(Robot robot)
        {

            this.robot = robot;
            this.stopwatch = new Stopwatch();
            this.stopwatch.Start();
            this.useRfid = true;
            if (this.useRfid)
            {
                this.rfid = new Rfid(robot);
            }
            this.leftMotor = new GrizzlyBear(robot, Watson.Motor.M0);
            this.rightMotor = new GrizzlyBear(robot, Watson.Motor.M1);
            this.sonar = new AnalogSonarDistanceSensor(robot, Watson.Analog.A5);

            this.leftEncoder = new GrizzlyEncoder(1, leftMotor, robot);
            this.rightEncoder = new GrizzlyEncoder(1, rightMotor, robot);

        }

        /// <summary>
        /// Main method which initializes the robot, and starts
        /// it running. Do not modify.
        /// </summary>
        public static void Main()
        {
            // Initialize robot
            Robot robot = new Robot("1", "COM4");
            Debug.Print("Code loaded successfully!");
            Supervisor supervisor = new Supervisor(new StudentCode(robot));
            supervisor.RunCode();
        }

        /// <summary>
        ///  The Robot to use.
        /// </summary>
        /// <returns>
        ///   Robot associated with this StudentCode.
        /// </returns>
        public Robot GetRobot()
        {
            return this.robot;
        }

        /// <summary>
        /// The robot will call this method every time it needs to run the
        /// user-controlled student code
        /// The StudentCode should basically treat this as a chance to read all the
        /// new PiEMOS analog/digital values and then use them to update the
        /// actuator states
        /// </summary>
        public void TeleoperatedCode()
        {
            Debug.Print("Tele-op " + this.stopwatch.ElapsedTime);

            this.rightMotor.Throttle = this.robot.PiEMOSAnalogVals[1];
            this.leftMotor.Throttle = -1 * this.robot.PiEMOSAnalogVals[3];

            this.robot.FeedbackAnalogVals[0] = this.rightMotor.Throttle;
            this.robot.FeedbackAnalogVals[1] = this.leftMotor.Throttle;

            if (this.sonar.Distance < 12)
            {
                Debug.Print("About to crash into a wall!");
            }

            if (this.useRfid)
            {
                this.ReportFieldItemType(this.rfid.CurrentItemScanned);
            }
        }

        /// <summary>
        /// The robot will call this method every time it needs to run the
        /// autonomous student code
        /// The StudentCode should basically treat this as a chance to change motors
        /// and servos based on non user-controlled input like sensors. But you
        /// don't need sensors, as this example demonstrates.
        /// </summary>
        public void AutonomousCode()
        {
            //float i = this.leftEncoder.Displacement;
            // this.leftMotor.Throttle = (int)i;
            //Debug.Print("Autonomous");


            //The simulator robot doesn't seem to have encoders 
            //and I haven't had time to test it on the real robot, so this hasn't been tested. 
            //I think it should make the robot drive around and do some turns, then stop. 
            //It's not very well designed, but hopefully it works.
            // - Patrick
            Debug.Print("Left Distance: " + this.leftEncoder.Displacement);
            Debug.Print("Right Distance: " + this.rightEncoder.Displacement);


            int flag = 0;

            while (flag == 0)
            {
                this.rightMotor.Throttle = 100;
                this.leftMotor.Throttle = 100;
                if (this.leftEncoder.Displacement > 5)
                {
                    flag = 1;
                }
            }
            while (flag == 1)
            {
                this.rightMotor.Throttle = 0;
                this.leftMotor.Throttle = 100;
                if (this.leftEncoder.Displacement > 1)
                {
                    flag = 2;
                }
            }
            while (flag == 2)
            {
                this.rightMotor.Throttle = 100;
                this.leftMotor.Throttle = 100;
                if (this.leftEncoder.Displacement > 5)
                {
                    flag = 3;
                }
            }
            while (flag == 3)
            {
                this.rightMotor.Throttle = 100;
                this.leftMotor.Throttle = 0;
                if (this.rightEncoder.Displacement > 1)
                {
                    flag = 4;
                }
            }

            while (flag == 4)
            {
                this.rightMotor.Throttle = 100;
                this.leftMotor.Throttle = 100;
                if (this.leftEncoder.Displacement > 5)
                {
                    flag = 5;
                }
            }
            while (flag == 5)
            {
                return;
            }


        }

        /// <summary>
        /// The robot will call this method periodically while it is disabled
        /// during the autonomous period. Actuators will not be updated during
        /// this time.
        /// </summary>
        public void DisabledAutonomousCode()
        {
            this.stopwatch.Reset(); // Restart stopwatch before start of autonomous
        }

        /// <summary>
        /// The robot will call this method periodically while it is disabled
        /// during the user  period. Actuators will not be updated
        /// during this time. 
        /// </summary>
        public void DisabledTeleoperatedCode()
        {
            if (stopwatch.ElapsedTime > 1 && rfid.CurrentItemScanned != null)
            {
                Debug.Print(rfid.CurrentItemScanned.GetHashCode().ToString());
                stopwatch.Restart();
            }
        }

        /// <summary>
        /// This is called whenever the supervisor disables studentcode.
        /// </summary>
        public void WatchdogReset()
        {
        }

        /// <summary>
        /// Send the GroupType of a FieldItem object to PiEMOS.
        /// Populates two indices of FeedbackDigitalVals.
        /// </summary>
        /// <param name="item">the FieldItem to send infotmaion about</param>
        /// <param name="index1">first index to use</param>
        /// <param name="index2">second index to use</param>
        private void ReportFieldItemType(FieldItem item, int index1 = 6, int index2 = 7)
        {
            bool feedback1;
            bool feedback2;

            if (item == null)
            {
                feedback1 = false;
                feedback2 = false;
            }
            else if (item.GroupType == FieldItem.PlusOneBox)
            {
                feedback1 = true;
                feedback2 = false;
            }
            else if (item.GroupType == FieldItem.TimesTwoBox)
            {
                feedback1 = true;
                feedback2 = true;
            }
            else
            {
                feedback1 = false;
                feedback2 = true;
            }

            this.robot.FeedbackDigitalVals[index1] = feedback1;
            this.robot.FeedbackDigitalVals[index2] = feedback2;
        }
    }
}
