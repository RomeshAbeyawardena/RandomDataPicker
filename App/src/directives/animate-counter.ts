import { DirectiveBinding } from "vue";
import { CountUp, CountUpOptions } from "countup.js";

export const AnimateCounterDirective = {
    name: "animateCounter",
    mounted(el:HTMLElement, binding:DirectiveBinding) { 
        new CountUp(el, binding.value.endValue, {
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
    },
    
}