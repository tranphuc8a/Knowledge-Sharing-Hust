import { library } from '@fortawesome/fontawesome-svg-core';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

import MIcon from '@/components/base/icons/MIcon.vue';
import MSpinner from '@/components/base/icons/MSpinner';

// Thêm tất cả các icons trong thư viện fas vào thư viện mà chúng ta sẽ sử dụng.
library.add(fas);

function registerComponets(app){
    // Đăng ký Global Component
    app.component('FontAwesomeIcon', FontAwesomeIcon);
    app.component('MIcon', MIcon);
    app.component('MSpinner', MSpinner);
}

export default registerComponets;
