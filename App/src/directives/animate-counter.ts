import { DirectiveBinding } from "vue";
import { CountUp, CountUpOptions } from "countup.js";

export enum easingFn {
    easeOutExpo,
    outQuintic,
    outCubic
}

export class AnimateCounterDirective {
    static default(): AnimateCounterDirective {
        return new AnimateCounterDirective();
    }

    readonly name: string = "animateCounter";
    countUp?: CountUp = undefined;
    getEasingFunction(easeFunction: easingFn) {
        switch (easeFunction) {
            case easingFn.easeOutExpo:
                return null;
            case easingFn.outQuintic:
                return (t: number, b: number, c: number, d: number): number => {
                    const ts = (t /= d) * t;
                    const tc = ts * t;
                    return b + c * (tc * ts + -5 * ts * ts + 10 * tc + -10 * ts + 5 * t);
                };
            case easingFn.outCubic:
                return (t: number, b: number, c: number, d: number): number => {
                    const ts = (t /= d) * t;
                    const tc = ts * t;
                    return b + c * (tc + -3 * ts + 3 * t);
                };
        }

    };
    mounted(el: HTMLElement, binding: DirectiveBinding): void {
        if (this.countUp != undefined) {
            this.countUp.update(binding.value.endValue);
            return;
        }

        this.countUp = new CountUp(el, binding.value.endValue, {
            duration: binding.value.duration,
            enableScrollSpy: binding.modifiers.enableScrollSpy,
            easingFn(t, b, c, d) {
                const ts = (t /= d) * t;
                const tc = ts * t;
                return b + c * (tc * ts + -5 * ts * ts + 10 * tc + -10 * ts + 5 * t);
            },
            useEasing: binding.modifiers.useEasing,
            startVal: binding.value.startValue
        } as CountUpOptions);
    }
}