import { createApp } from 'vue'
import App from './App.vue'
import { createPinia } from "pinia"
import PrimeVue from 'primevue/config';
import "primevue/resources/themes/bootstrap4-dark-blue/theme.css";
import 'primeicons/primeicons.css';
import 'primeflex/primeflex.css';
createApp(App)
    .use(createPinia())
    .use(PrimeVue)
    .mount('#app');
