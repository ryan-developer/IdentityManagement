import { __decorate } from "tslib";
import Vue from 'vue';
import { Component } from 'vue-property-decorator';
let EventBus = class EventBus extends Vue {
    RegisterListener(eventName, callback) {
        this.$on(eventName, callback);
    }
    NotifyValueChangd(eventName, value) {
        this.$emit(eventName, value);
    }
};
EventBus = __decorate([
    Component({
        name: 'event-bus',
    })
], EventBus);
export { EventBus };
export default new EventBus();
//# sourceMappingURL=event-bus.js.map