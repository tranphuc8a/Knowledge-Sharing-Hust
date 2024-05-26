

<template>

    <div class="card"
        style="padding: 16px;"
    >
        <div class="">
            <div class="skeleton"
                style="width: 100%; aspect-ratio: 2 / 1; border-radius: 8px; "
            >
            </div>
        </div>

        <div
            style="margin-top: 16px;"
        >
            <div>
                <div class="skeleton"
                    style="width: 60%; height: 28px;">
                </div>
                <div class="skeleton"
                    style="width: 40%; height: 28px; margin-top: 4px;">
                </div>
            </div>
            <div
                style="display: flex; justify-content: flex-end; margin-top: 16px; gap: 16px;"
            >
                <!-- <div class="skeleton"
                    style="width: 40%; height: 32px; flex: 1"
                >
                </div> -->
                <div class="skeleton"
                    style="width: 40%; height: 32px; flex-grow: 0"
                >
                </div>
            </div>
        </div>

        
    </div>
</template>



<script>

export default {
    name: 'CourseRegisteredCardSkeleton',
}

</script>


