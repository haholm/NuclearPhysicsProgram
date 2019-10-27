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
        public void TransitionEffect(Action<double> PropertyUpdater, double from, double to, double seconds, double multiplier) {
            double stepsPerSecond = 1 / seconds;
            TransitionEffectAnimation animation = new TransitionEffectAnimation(PropertyUpdater, from, to, stepsPerSecond, multiplier);
            CompositionTarget.Rendering += animation.PerformStep;
        }

        public async Task AsyncTransitionEffect(Action<double> PropertyUpdater, double from, double to, double seconds, double multiplier) {
            double stepsPerSecond = (float)1 / seconds;
            TransitionEffectAnimation animation = new TransitionEffectAnimation(PropertyUpdater, from, to, stepsPerSecond, multiplier);
            CompositionTarget.Rendering += animation.PerformStep;
            while (!animation.Finished)
                await Task.Delay((int)(seconds * 1000));
        }
    }

    public class TransitionEffectAnimation {
        private readonly Action<double> PropertyUpdater;
        private readonly double from;
        private readonly double to;
        private readonly double step;
        private readonly double multiplier;
        private double progress;
        private DateTime currentTime;
        private DateTime previousTime;

        public bool Finished { get => progress == to; }

        public TransitionEffectAnimation(Action<double> PropertyUpdater, double from, double to, double step, double multiplier) {
            this.PropertyUpdater = PropertyUpdater;
            this.from = from;
            this.to = to;
            this.step = step;
            this.multiplier = multiplier;
            progress = from;
            currentTime = new DateTime(0);
            previousTime = new DateTime(0);
        }

        public void PerformStep(object sender, EventArgs args) {
            double deltaTime = CalculateDeltaTime();

            if (from < to)
                Increase(deltaTime);
            else if (from > to)
                Decrease(deltaTime);

            PropertyUpdater(progress);
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
            if (progress < to)
                progress += GetSteppedProgress(deltaTime);
            else if (progress > to || progress == to)
                StopAnimating();
        }

        private void Decrease(double deltaTime) {
            if (progress > to) 
                progress -= GetSteppedProgress(deltaTime);
            else if (progress < to || progress == to) 
                StopAnimating();
        }

        private double GetSteppedProgress(double deltaTime) => (step + Math.Pow(progress, multiplier)) * deltaTime;

        private void StopAnimating() {
            progress = to;
            CompositionTarget.Rendering -= PerformStep;
        }
    }
}

