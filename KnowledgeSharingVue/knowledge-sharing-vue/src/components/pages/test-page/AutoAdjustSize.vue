
<template>
    <div class="p-bgc">
        <div class="p-bgc__header">
            <h5>Auto Adjust Size</h5>
        </div>
        <div class="p-bgc__content">
            <div class="p-adjust-follow-width">
                <div class="p-parent">
                    <div class="p-child-1">
                        Child 1
                    </div>
                    <div class="p-child-2">
                        Child 2
                    </div>
                </div>
            </div>
            <div class="p-adjust-follow-height">
                <div class="p-parent">
                    <div class="p-child-1">
                        Child 1
                    </div>
                    <div class="p-child-2">
                        Child 2
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>

export default {
    name: "AutoAdjustSize",
    data(){
        return {
        }
    },
    components: {
    },
    mounted(){
    },
    methods: {
        
    },
    props: {

    },
}
</script>



<style>
.p-bgc{
    background-color: pink;
    width: 100%;
    padding: 16px;
    
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-between;
    align-items: center;
    gap: 12px;
}

.p-bgc__content{
    width: 100%;
    display: flex;
    flex-flow: column nowrap;
    justify-content: space-between;
    align-items: flex-start;
    gap: 12px;
}

.p-parent{
    width: fit-content;
    height: fit-content;
    background-color: green;
    display: flex;
    justify-content: flex-start;
    align-items: flex-start;
}

.p-child-1{
    background-color: orange;
    width: 500px;
    height: 300px;
    flex-grow: 0;
    flex-shrink: 0;
}

.p-child-2{
    background-color: grey;
    align-self: stretch;
}

.p-adjust-follow-width .p-parent{
    flex-flow: column nowrap;
}
.p-adjust-follow-height .p-parent{
    flex-flow: row nowrap;
}
</style>


