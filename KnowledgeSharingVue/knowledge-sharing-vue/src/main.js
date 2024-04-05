import { createApp } from 'vue';
import App from '@/App.vue';
import { createMyRouter } from '@/js/services/router';
import globalProperties from '@/js/services/global-properties';


const app = createApp(App);
const router = createMyRouter(app);

app.use(router);

app.config.globalProperties = {
    ...app.config.globalProperties, 
    ...globalProperties
}



app.mount('#app');

