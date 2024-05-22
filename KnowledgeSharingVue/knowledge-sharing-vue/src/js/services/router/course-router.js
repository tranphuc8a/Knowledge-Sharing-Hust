

export default [
    {
        path: '/course/:courseId',
        name: 'course',
        component: ProfilePage,
        meta: {
            requiredAuth: false
        },
        children: [
            {
                path: '',
                name: 'course-home-page',
                component: null,
            },
            {
                path: '',
                name: 'course-home-page',
                component: null,
            }
        ]
    }
]

