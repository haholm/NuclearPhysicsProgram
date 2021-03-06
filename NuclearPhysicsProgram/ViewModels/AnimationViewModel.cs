﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NuclearPhysicsProgram.ViewModels {
    public class AnimationViewModel {
        private Dictionary<(Action<double?> PropertyUpdater, double from, double to, double stepsPerSecond, double multiplier), TransitionAnimation> previousAnimations;

        public AnimationViewModel() {
            previousAnimations = new Dictionary<(Action<double?>, double, double, double, double), TransitionAnimation>();
        }

        public void Transition(Action<double?> PropertyUpdater, double from, double to, double seconds, double multiplier) {
            TransitionAnimation animation = ConstructTransitionEffectAnimation(PropertyUpdater, from, to, seconds, multiplier);
            animation.Start();
        }

        public async Task AsyncTransition(Action<double?> PropertyUpdater, double from, double to, double seconds, double multiplier) {
            TransitionAnimation animation = ConstructTransitionEffectAnimation(PropertyUpdater, from, to, seconds, multiplier);
            animation.Start();
            while (!animation.Finished)
                await Task.Delay((int)(seconds * 1000));
        }

        private TransitionAnimation ConstructTransitionEffectAnimation(Action<double?> PropertyUpdater, double from, double to, double seconds, double multiplier) {
            var args = (PropertyUpdater, from, to, seconds, multiplier);
            if (previousAnimations.TryGetValue(args, out var existingAnimation))
                return existingAnimation;

            var animation = new TransitionAnimation(PropertyUpdater, from, to, seconds, multiplier);
            previousAnimations.Add(args, animation);
            return animation;
        }
    }

    public class TransitionAnimation {
        private readonly Action<double?> PropertyUpdater;
        private readonly double from;
        private readonly double to;
        private readonly double step;
        private readonly double multiplier;
        private double progress;
        private DateTime currentTime;
        private DateTime previousTime;
        private double accumulatedTime;

        public bool Finished { get => progress == to; }

        public TransitionAnimation(Action<double?> PropertyUpdater, double from, double to, double seconds, double multiplier) {
            this.PropertyUpdater = PropertyUpdater;
            this.from = from;
            this.to = to;
            if (from < to && to != 0)
                this.step = Math.Abs(to) / seconds;
            else if (from != 0)
                this.step = Math.Abs(from) / seconds;

            this.multiplier = multiplier;
        }

        public void Start() {
            progress = from;
            currentTime = new DateTime(0);
            previousTime = new DateTime(0);
            CompositionTarget.Rendering += PerformStep;
        }

        public void Stop() {
            progress = to;
            CompositionTarget.Rendering -= PerformStep;
        }

        private void PerformStep(object sender, EventArgs args) {
            double deltaTime = CalculateDeltaTime();
            accumulatedTime += deltaTime;

            if (from < to)
                Increase(deltaTime);
            else if (from > to)
                Decrease(deltaTime);

            //update property only ~60x/s
            if (accumulatedTime > 0.016) {
                accumulatedTime = 0;
                PropertyUpdater(progress);
            }
        }

        private double CalculateDeltaTime() {
            currentTime = DateTime.Now;
            if (previousTime.Ticks == 0)
                previousTime = currentTime;
            TimeSpan deltaTime = currentTime - previousTime;
            previousTime = currentTime;
            return deltaTime.TotalSeconds;
        }

        private void Increase(double deltaTime) {
            double newProgress = progress + (CalculateChange() * deltaTime);
            if (newProgress < to) { 
                progress = newProgress;
                return;
            }
            
            Stop();
        }

        private void Decrease(double deltaTime) {
            double newProgress = progress - (CalculateChange() * deltaTime);
            if (newProgress > to) {
                progress = newProgress;
                return;
            }
            
            Stop();
        }

        private double CalculateChange() {
            if (multiplier == 1)
                return step;
            else
                return step + Math.Pow(Math.Abs(progress), multiplier);
        }
    }
}

