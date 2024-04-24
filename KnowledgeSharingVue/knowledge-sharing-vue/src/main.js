import { createApp } from 'vue';
import App from '@/App.vue';
import { createMyRouter } from '@/js/services/router';
import registerGlobalProperties from './js/services/global-properties';
import registerComponets from './js/services/register-global-component';

const app = createApp(App);
registerComponets(app);
registerGlobalProperties(app);
const router = createMyRouter(app);

app.use(router);



app.mount('#app');

