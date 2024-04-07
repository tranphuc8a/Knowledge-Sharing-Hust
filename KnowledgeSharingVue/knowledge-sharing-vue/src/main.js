import { createApp } from 'vue';
import App from '@/App.vue';
import { createMyRouter } from '@/js/services/router';
import globalProperties from '@/js/services/global-properties';
import registerComponets from './js/services/register-global-component';

const app = createApp(App);
registerComponets(app);
const router = createMyRouter(app);

app.use(router);

app.config.globalProperties = {
    ...app.config.globalProperties, 
    ...globalProperties
}

app.mount('#app');

