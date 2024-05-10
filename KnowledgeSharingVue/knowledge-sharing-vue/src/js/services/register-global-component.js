import { library } from '@fortawesome/fontawesome-svg-core';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { far } from '@fortawesome/free-regular-svg-icons';
import { fab } from '@fortawesome/free-brands-svg-icons';

import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import 'ant-design-vue/dist/reset.css';
import Antd from 'ant-design-vue';

import MIcon from '@/components/base/icons/MIcon.vue';
import MSpinner from '@/components/base/icons/MSpinner';
import MActionIcon from '@/components/base/icons/MActionIcon'

// Thêm tất cả các icons trong thư viện fas vào thư viện mà chúng ta sẽ sử dụng.
library.add(fas, far);
library.add(fab);

function registerComponets(app){
    // Đăng ký Global Component
    app.component('FontAwesomeIcon', FontAwesomeIcon);
    app.component('MIcon', MIcon);
    app.component('MActionIcon', MActionIcon);
    app.component('MSpinner', MSpinner);
    app.use(Antd);
}

export default registerComponets;
