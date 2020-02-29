import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component({
    name: 'event-bus',
})
export class EventBus extends Vue {
    public RegisterListener<TResultType>(eventName: string, callback: (value: TResultType) => void) {
        this.$on(eventName, callback);
    }
    public NotifyValueChangd<TValueType>(eventName: string, value: TValueType) {
        this.$emit(eventName, value);
    }
}

export default new EventBus();
