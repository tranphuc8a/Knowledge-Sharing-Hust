

export default{
    methods: {
        getCategories() {
            if (this.post?.Categories != null
                && this.post.Categories.length > 0){
                return this.post.Categories;
            }
            return this.defaultCategoriesList;
        },
        
        async resolveOnClickComment(){
            try {
                if (this.$refs['comment-list'] != null){
                    await this.$refs['comment-list'].focus();
                }
            } catch (e){
                console.error(e);
            }
        },

        async forceUpdateCommentList(){
            try {
                if (this.isShowComment == true){
                    this.isShowComment = false;
                    this.$nextTick(() => {
                        this.isShowComment = true;
                    });
                }
            } catch (e){
                console.error(e);
            }
        }
    },

    props: {

    },
    
}