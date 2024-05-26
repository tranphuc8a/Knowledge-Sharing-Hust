

const scrollHandler = {
    /**
     * Xu ly su kien onScroll
     * @param {*} scrollContainer - container chua noi dung can xu ly
     * @param {*} callback - ham callback kich hoat load more
     * @param {*} averageItemHeight - chieu cao trung binh cua 1 item, default value is 300px
     * @param {*} leftItemNumber - so luong item con lai de load them, default value is 5
     * @Created PhucTV 25/5/24
     * @Modified None
     */
    async resolve(scrollContainer, callback, averageItemHeight = 300, leftItemNumber = 5){
        try {
            if (scrollContainer == null || callback == null) {
                return;
            }
            let scrollHeight = scrollContainer.scrollHeight;
            let scrollTop = scrollContainer.scrollTop;
            let clientHeight = scrollContainer.clientHeight;
            let scrollPosition = clientHeight + scrollTop;
            // console.log("scrollTop: " + scrollTop + " scrollHeight: " + scrollHeight + " clientHeight: " + clientHeight);

            if (scrollHeight - scrollPosition < averageItemHeight * leftItemNumber){
                // console.log("Load more post");
                await callback();
            }
        } catch (error){
            console.error(error);
        }
    }
}

export default scrollHandler;