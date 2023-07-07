<template >
    <div class="container">
        <h1>{{ page.pageTitle }}</h1>
        <p>{{ page.content }}</p>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    export default defineComponent({
        props: ['page']
    })
</script>

<style scoped>
    .container{
        text-align: center;
    }
</style>
