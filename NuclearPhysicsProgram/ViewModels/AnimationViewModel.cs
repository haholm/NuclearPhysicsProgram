using System;
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
        public void TransitionEffect(Action<double?> PropertyUpdater, double from, double to, double seconds, double multiplier) {
            double stepsPerSecond = 1 / seconds;
            TransitionEffectAnimation animation = new TransitionEffectAnimation(PropertyUpdater, from, to, stepsPerSecond, multiplier);
            animation.Start();
        }

        public async Task AsyncTransitionEffect(Action<double?> PropertyUpdater, double from, double to, double seconds, double multiplier) {
            double stepsPerSecond = (float)1 / seconds;
            TransitionEffectAnimation animation = new TransitionEffectAnimation(PropertyUpdater, from, to, stepsPerSecond, multiplier);
            animation.Start();
            while (!animation.Finished)
                await Task.Delay((int)(seconds * 1000));
        }
    }

    public class TransitionEffectAnimation {
        private readonly Action<double?> PropertyUpdater;
        private readonly double from;
        private readonly double to;
        private readonly double step;
        private readonly double multiplier;
        private double progress;
        private DateTime currentTime;
        private DateTime previousTime;
        private double accumulatedTime;

        public double Progress { get => progress; }
        public bool Finished { get => progress == to; }

        public TransitionEffectAnimation(Action<double?> PropertyUpdater, double from, double to, double step, double multiplier) {
            this.PropertyUpdater = PropertyUpdater;
            this.from = from;
            this.to = to;
            this.step = step;
            this.multiplier = multiplier;
            progress = from;
            currentTime = new DateTime(0);
            previousTime = new DateTime(0);
        }

        public void Start() => CompositionTarget.Rendering += PerformStep;

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
            if (accumulatedTime > 0.017) {
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
            double newProgress = progress + (step + Math.Pow(Math.Abs(progress), multiplier)) * deltaTime;
            if (newProgress < to) { 
                progress = newProgress;
                return;
            }
            
            Stop();
        }

        private void Decrease(double deltaTime) {
            double newProgress = progress - (step + Math.Pow(Math.Abs(progress), multiplier)) * deltaTime;
            if (newProgress > to) {
                progress = newProgress;
                return;
            }
            
            Stop();
        }
    }
}

