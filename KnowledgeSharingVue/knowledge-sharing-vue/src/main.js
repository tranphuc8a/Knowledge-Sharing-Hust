import { createApp } from 'vue';
import App from '@/App.vue';
import { createMyRouter } from '@/js/services/router';
import registerGlobalProperties from './js/services/global-properties';
import registerComponets from './js/services/register-global-component';
import VueScrollTo from 'vue-scrollto';
// import VueSmoothScroll from 'vue2-smooth-scroll';


const app = createApp(App);
registerComponets(app);
registerGlobalProperties(app);
const router = createMyRouter(app);

app.use(router);
app.use(VueScrollTo, {
    duration: 1000,
    easing: 'ease'
});
// app.use(VueSmoothScroll);


app.mount('#app');

